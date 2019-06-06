﻿using System;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;

using Loxodon.Log;
#if UNITY_IOS
using Loxodon.Framework.Binding.Expressions;
#endif

namespace Loxodon.Framework.Binding.Paths
{
    public class PathExpressionVisitor
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(PathExpressionVisitor));

        private readonly List<Path> list = new List<Path>();

        public List<Path> Paths { get { return list; } }

        public virtual Expression Visit(Expression expression)
        {
            if (expression == null)
                return null;

            var bin = expression as BinaryExpression;
            if (bin != null)
                return VisitBinary(bin);

            var cond = expression as ConditionalExpression;
            if (cond != null)
                return VisitConditional(cond);

            var constant = expression as ConstantExpression;
            if (constant != null)
                return VisitConstant(constant);

            var lambda = expression as LambdaExpression;
            if (lambda != null)
                return VisitLambda(lambda);

            var listInit = expression as ListInitExpression;
            if (listInit != null)
                return VisitListInit(listInit);

            var member = expression as MemberExpression;
            if (member != null)
                return VisitMember(member);

            var memberInit = expression as MemberInitExpression;
            if (memberInit != null)
                return VisitMemberInit(memberInit);

            var methodCall = expression as MethodCallExpression;
            if (methodCall != null)
                return VisitMethodCall(methodCall);

            var newExpr = expression as NewExpression;
            if (newExpr != null)
                return VisitNew(newExpr);

            var newArrayExpr = expression as NewArrayExpression;
            if (newArrayExpr != null)
                return VisitNewArray(newArrayExpr);

            var param = expression as ParameterExpression;
            if (param != null)
                return VisitParameter(param);

            var typeBinary = expression as TypeBinaryExpression;
            if (typeBinary != null)
                return VisitTypeBinary(typeBinary);

            var unary = expression as UnaryExpression;
            if (unary != null)
                return VisitUnary(unary);

            var invocation = expression as InvocationExpression;
            if (invocation != null)
                return VisitInvocation(invocation);

            throw new NotSupportedException("Expressions of type " + expression.Type + " are not supported.");
        }

        protected virtual void Visit(IList<Expression> nodes)
        {
            if (nodes == null || nodes.Count <= 0)
                return;

            foreach (Expression expression in nodes)
                Visit(expression);
        }

        protected virtual Expression VisitLambda(LambdaExpression node)
        {
            if (node.Parameters != null)
                Visit(node.Parameters.Select(p => (Expression)p).ToList());
            return Visit(node.Body);
        }

        protected virtual Expression VisitBinary(BinaryExpression node)
        {
            List<Expression> list = new List<Expression>() { node.Left, node.Right, node.Conversion };
            this.Visit(list);
            return null;
        }

        protected virtual Expression VisitConditional(ConditionalExpression node)
        {
            List<Expression> list = new List<Expression>() { node.IfFalse, node.IfTrue, node.Test };
            this.Visit(list);
            return null;
        }

        protected virtual Expression VisitConstant(ConstantExpression node)
        {
            return null;
        }

        protected virtual void VisitElementInit(ElementInit init)
        {
            if (init == null)
                return;

            this.Visit(init.Arguments);
        }

        protected virtual Expression VisitListInit(ListInitExpression node)
        {
            if (node.Initializers != null)
            {
                foreach (ElementInit init in node.Initializers)
                {
                    VisitElementInit(init);
                }
            }
            return Visit(node.NewExpression);
        }

        protected virtual Expression VisitMember(MemberExpression node)
        {
            this.Visit(ParseMemberPath(node, null, this.list));
            return null;
        }

        protected virtual Expression VisitInvocation(InvocationExpression node)
        {
            this.Visit(node.Arguments);
            return this.Visit(node.Expression);
        }

        protected virtual Expression VisitMemberInit(MemberInitExpression expr)
        {
            return Visit(expr.NewExpression);
        }

        protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding)
        {
            return null;
        }

        protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
        {
            return null;
        }

        protected virtual Expression VisitMethodCall(MethodCallExpression node)
        {
            this.Visit(ParseMemberPath(node, null, this.list));
            return null;
        }

        protected virtual Expression VisitNew(NewExpression expr)
        {
            Visit(expr.Arguments);
            return null;
        }

        protected virtual Expression VisitNewArray(NewArrayExpression node)
        {
            this.Visit(node.Expressions);
            return null;
        }

        protected virtual Expression VisitParameter(ParameterExpression node)
        {
            return null;
        }

        protected virtual Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            return Visit(node.Expression);
        }

        protected virtual Expression VisitUnary(UnaryExpression node)
        {
            return Visit(node.Operand);
        }

        private static Expression ConvertMemberAccessToConstant(Expression argument)
        {
            if (argument is ConstantExpression)
                return argument;

            try
            {
                var boxed = Expression.Convert(argument, typeof(object));
#if !UNITY_IOS
                var fun = Expression.Lambda<Func<object>>(boxed).Compile();
                var constant = fun();
#else
                var fun = (Func<object[],object>)Expression.Lambda<Func<object>>(boxed).DynamicCompile();
                var constant = fun(new object[] { });
#endif

                var constExpr = Expression.Constant(constant);
                return constExpr;
            }
            catch (Exception e)
            {
                throw e;
                //if (log.IsWarnEnabled)
                //    log.Warn("Failed to evaluate member expression.", e);
            }
        }

        private IList<Expression> ParseMemberPath(Expression expression, Path path, IList<Path> list)
        {
            if (expression.NodeType != ExpressionType.MemberAccess && expression.NodeType != ExpressionType.Call)
                throw new Exception();

            List<Expression> result = new List<Expression>();

            Expression current = expression;
            while (current != null && (current is MemberExpression || current is MethodCallExpression || current is ParameterExpression || current is ConstantExpression))
            {
                if (current is MemberExpression)
                {
                    if (path == null)
                    {
                        path = new Path();
                        list.Add(path);
                    }

                    MemberExpression me = (MemberExpression)current;
                    var field = me.Member as FieldInfo;
                    if (field != null)
                    {
                        if (field.IsStatic)
                        {
                            path.Prepend(new MemberNode(field));                         
                            path.Prepend(new TypeNode(field.DeclaringType));
                        }
                        else
                        {
                            path.Prepend(new MemberNode(field));
                        }
                    }

                    var property = me.Member as PropertyInfo;
                    if (property != null)
                    {
                        if (property.IsStatic())
                        {                           
                            path.Prepend(new MemberNode(property));
                            path.Prepend(new TypeNode(property.DeclaringType));
                        }
                        else
                        {
                            path.Prepend(new MemberNode(property));
                        }
                    }

                    current = me.Expression;
                }
                else if (current is MethodCallExpression)
                {
                    MethodCallExpression mc = (MethodCallExpression)current;
                    if (mc.Method.Name.Equals("get_Item") && mc.Arguments.Count == 1)
                    {
                        if (path == null)
                        {
                            path = new Path();
                            list.Add(path);
                        }

                        var argument = mc.Arguments[0];
                        if (!(argument is ConstantExpression))
                        {
                            argument = ConvertMemberAccessToConstant(argument);
                            //throw new NotSupportedException();
                        }

                        object value = (argument as ConstantExpression).Value;
                        if (value is string)
                        {
                            path.PrependIndexed((string)value);
                        }
                        else if (value is Int32)
                        {
                            path.PrependIndexed((int)value);
                        }

                        current = mc.Object;
                    }
                    else {
                        current = null;
                        result.AddRange(mc.Arguments);
                        result.Add(mc.Object);
                    }
                }
                else if (current is ParameterExpression)
                {
                    //ParameterExpression e = (ParameterExpression)current;
                    //path.PrependProperty(e.Name);
                    current = null;
                }
                else if (current is ConstantExpression)
                {
                    //ConstantExpression e = (ConstantExpression)current;
                    //path.PrependProperty (e.Value.ToString());
                    current = null;
                }
            }

            if (current != null)
                result.Add(current);

            return result;
        }
    }
}
