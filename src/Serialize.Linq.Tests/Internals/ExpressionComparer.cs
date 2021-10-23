#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Serialize.Linq.Tests.Internals
{
    internal class ExpressionComparer
    {
        public virtual bool AreEqual(Expression x, Expression y)
        {
            if (Object.ReferenceEquals(x, y))
                return true;
            if (x.NodeType != y.NodeType)
                return false;

            switch (x.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                case ExpressionType.UnaryPlus:
                case ExpressionType.PreDecrementAssign:
                case ExpressionType.PreIncrementAssign:
                case ExpressionType.PostDecrementAssign:
                case ExpressionType.PostIncrementAssign:
                case ExpressionType.Throw:
                    return this.AreEqualUnary((UnaryExpression)x, (UnaryExpression)y);
                case ExpressionType.Add:
                case ExpressionType.AddAssign:
                case ExpressionType.AddAssignChecked:
                case ExpressionType.AddChecked:
                case ExpressionType.Assign:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractAssign:
                case ExpressionType.SubtractAssignChecked:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyAssign:
                case ExpressionType.MultiplyAssignChecked:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.DivideAssign:
                case ExpressionType.Modulo:
                case ExpressionType.ModuloAssign:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.Power:
                    return this.AreEqualBinary((BinaryExpression)x, (BinaryExpression)y);
                case ExpressionType.TypeIs:
                case ExpressionType.TypeEqual:
                    return this.AreEqualTypeBinary((TypeBinaryExpression)x, (TypeBinaryExpression)y);
                case ExpressionType.Conditional:
                    return this.AreEqualConditional((ConditionalExpression)x, (ConditionalExpression)y);
                case ExpressionType.Constant:
                    return this.AreEqualConstant((ConstantExpression)x, (ConstantExpression)y);
                case ExpressionType.Parameter:
                    return this.AreEqualParameter((ParameterExpression)x, (ParameterExpression)y);
                case ExpressionType.MemberAccess:
                    return this.AreEqualMemberAccess((MemberExpression)x, (MemberExpression)y);
                case ExpressionType.Call:
                    return this.AreEqualMethodCall((MethodCallExpression)x, (MethodCallExpression)y);
                case ExpressionType.Lambda:
                    return this.AreEqualLambda((LambdaExpression)x, (LambdaExpression)y);
                case ExpressionType.New:
                    return this.AreEqualNew((NewExpression)x, (NewExpression)y);
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return this.AreEqualNewArray((NewArrayExpression)x, (NewArrayExpression)y);
                case ExpressionType.Invoke:
                    return this.AreEqualInvocation((InvocationExpression)x, (InvocationExpression)y);
                case ExpressionType.MemberInit:
                    return this.AreEqualMemberInit((MemberInitExpression)x, (MemberInitExpression)y);
                case ExpressionType.ListInit:
                    return this.AreEqualListInit((ListInitExpression)x, (ListInitExpression)y);
                case ExpressionType.Block:
                    return this.AreEqualBlock((BlockExpression)x, (BlockExpression)y);
                case ExpressionType.Goto:
                    return this.AreEqualGoto((GotoExpression)x, (GotoExpression)y);
                case ExpressionType.Label:
                    return this.AreEqualLabel((LabelExpression)x, (LabelExpression)y);
                case ExpressionType.Loop:
                    return this.AreEqualLoop((LoopExpression)x, (LoopExpression)y);
                case ExpressionType.Switch:
                    return this.AreEqualSwitch((SwitchExpression)x, (SwitchExpression)y);
                case ExpressionType.Try:
                    return this.AreEqualTry((TryExpression)x, (TryExpression)y);
                default:
                    throw new Exception(String.Format("Unhandled expression type: '{0}'", x.NodeType));
            }
        }

        protected virtual bool AreEqualBinding(MemberBinding x, MemberBinding y)
        {
            if (x.BindingType != y.BindingType)
                return false;

            switch (x.BindingType)
            {
                case MemberBindingType.Assignment:
                    return this.AreEqualMemberAssignment((MemberAssignment)x, (MemberAssignment)y);
                case MemberBindingType.MemberBinding:
                    return this.AreEqualMemberMemberBinding((MemberMemberBinding)x, (MemberMemberBinding)y);
                case MemberBindingType.ListBinding:
                    return this.AreEqualMemberListBinding((MemberListBinding)x, (MemberListBinding)y);
                default:
                    throw new Exception(String.Format("Unhandled binding type '{0}'", y.BindingType));
            }
        }

        protected virtual bool AreEqualElementInitializer(ElementInit x, ElementInit y)
        {
            return this.AreEqualExpressionList(x.Arguments, y.Arguments);
        }

        protected virtual bool AreEqualLabelTarget(LabelTarget x, LabelTarget y)
        {
            return x == y
                || x.Type == y.Type;
        }

        protected virtual bool AreEqualUnary(UnaryExpression x, UnaryExpression y)
        {
            return this.AreEqual(x.Operand, y.Operand);
        }

        protected virtual bool AreEqualBinary(BinaryExpression x, BinaryExpression y)
        {
            return this.AreEqual(x.Left, y.Left)
                && this.AreEqual(x.Right, y.Right)
                && this.AreEqual(x.Conversion, y.Conversion);
        }

        protected virtual bool AreEqualTypeBinary(TypeBinaryExpression x, TypeBinaryExpression y)
        {
            return x.TypeOperand == y.TypeOperand
                && this.AreEqual(x.Expression, y.Expression);
        }

        protected virtual bool AreEqualConstant(ConstantExpression x, ConstantExpression y)
        {
            return x.Type == y.Type
                && (Object.ReferenceEquals(x.Value, y.Value) 
                    || x.Value.Equals(y.Value) 
                    || PublicInstancePropertiesEqual(x.Value, y.Value));
        }

        protected virtual bool AreEqualMethodInfo(MethodInfo x, MethodInfo y)
        {
            return x == y
                || (x.Name == y.Name
                    && x.Attributes == y.Attributes
                    && x.DeclaringType == y.DeclaringType
                    && x.ReturnType == y.ReturnType);
        }

        protected virtual bool AreEqualConditional(ConditionalExpression x, ConditionalExpression y)
        {
            return this.AreEqual(x.Test, y.Test)
                && this.AreEqual(x.IfTrue, y.IfTrue)
                && this.AreEqual(x.IfFalse, y.IfFalse);
        }

        protected virtual bool AreEqualParameter(ParameterExpression x, ParameterExpression y)
        {
            return x == y
                || (x.Type == y.Type
                    && (Object.ReferenceEquals(x.Name, y.Name) || x.Name.Equals(y.Name)));
        }

        protected virtual bool AreEqualMemberAccess(MemberExpression x, MemberExpression y)
        {
            return this.AreEqual(x.Expression, y.Expression);
        }

        protected virtual bool AreEqualMethodCall(MethodCallExpression x, MethodCallExpression y)
        {
            var isEqual = this.AreEqual(x.Object, y.Object);
            if (isEqual) isEqual = this.AreEqualExpressionList(x.Arguments, y.Arguments);
            return isEqual;
        }

        protected virtual bool AreEqualExpressionList(ReadOnlyCollection<Expression> x, ReadOnlyCollection<Expression> y)
        {
            var isEqual = x.Count.Equals(y.Count);
            for (var i = 0; isEqual && i < x.Count; ++i)
                isEqual = this.AreEqual(x[i], y[i]);
            return isEqual;
        }

        protected virtual bool AreEqualCaseList(ReadOnlyCollection<SwitchCase> x, ReadOnlyCollection<SwitchCase> y)
        {
            var isEqual = x.Count.Equals(y.Count);
            for (var i = 0; isEqual && i < x.Count; ++i)
                isEqual = this.AreEqualCase(x[i], y[i]);
            return isEqual;
        }

        protected virtual bool AreEqualCase(SwitchCase x, SwitchCase y)
        {
            return this.AreEqual(x.Body, y.Body)
                && this.AreEqualExpressionList(x.TestValues, y.TestValues);
        }

        protected virtual bool AreEqualCatchBlockList(ReadOnlyCollection<CatchBlock> x, ReadOnlyCollection<CatchBlock> y)
        {
            var isEqual = x.Count.Equals(y.Count);
            for (var i = 0; isEqual && i < x.Count; ++i)
                isEqual = this.AreEqualCatchBlock(x[i], y[i]);
            return isEqual;
        }

        protected virtual bool AreEqualCatchBlock(CatchBlock x, CatchBlock y)
        {
            return this.AreEqual(x.Body, y.Body)
                && this.AreEqualParameter(x.Variable, y.Variable)
                && this.AreEqual(x.Filter, y.Filter);
        }

        protected virtual bool AreEqualMemberAssignment(MemberAssignment x, MemberAssignment y)
        {
            return this.AreEqual(x.Expression, y.Expression);
        }

        protected virtual bool AreEqualMemberMemberBinding(MemberMemberBinding x, MemberMemberBinding y)
        {
            return this.AreEqualBindingList(x.Bindings, y.Bindings);
        }

        protected virtual bool AreEqualMemberListBinding(MemberListBinding x, MemberListBinding y)
        {
            return this.AreEqualElementInitializerList(x.Initializers, y.Initializers);
        }

        protected virtual bool AreEqualBindingList(ReadOnlyCollection<MemberBinding> x, ReadOnlyCollection<MemberBinding> y)
        {
            var isEqual = x.Count.Equals(y.Count);
            for (var i = 0; isEqual && i < x.Count; ++i)
                isEqual = this.AreEqualBinding(x[i], y[i]);
            return isEqual;
        }

        protected virtual bool AreEqualElementInitializerList(ReadOnlyCollection<ElementInit> x, ReadOnlyCollection<ElementInit> y)
        {
            var isEqual = x.Count.Equals(y.Count);
            for (var i = 0; isEqual && i < x.Count; ++i)
                isEqual = this.AreEqualElementInitializer(x[i], y[i]);
            return isEqual;
        }

        protected virtual bool AreEqualParameterList(ReadOnlyCollection<ParameterExpression> x, ReadOnlyCollection<ParameterExpression> y)
        {
            var isEqual = x.Count.Equals(y.Count);
            for (var i = 0; isEqual && i < x.Count; ++i)
                isEqual = this.AreEqualParameter(x[i], y[i]);
            return isEqual;
        }

        protected virtual bool AreEqualLambda(LambdaExpression x, LambdaExpression y)
        {
            return this.AreEqual(x.Body, y.Body)
                && this.AreEqualParameterList(x.Parameters, y.Parameters);
        }

        protected virtual bool AreEqualNew(NewExpression x, NewExpression y)
        {
            return this.AreEqualExpressionList(x.Arguments, y.Arguments);
        }

        protected virtual bool AreEqualMemberInit(MemberInitExpression x, MemberInitExpression y)
        {
            return this.AreEqualNew(x.NewExpression, y.NewExpression)
                && this.AreEqualBindingList(x.Bindings, y.Bindings);
        }

        protected virtual bool AreEqualListInit(ListInitExpression x, ListInitExpression y)
        {
            return this.AreEqualNew(x.NewExpression, y.NewExpression)
                && this.AreEqualElementInitializerList(x.Initializers, y.Initializers);
        }

        protected virtual bool AreEqualBlock(BlockExpression x, BlockExpression y)
        {
            return x.Type == y.Type
                && this.AreEqualExpressionList(x.Expressions, y.Expressions)
                && this.AreEqualParameterList(x.Variables, y.Variables)
                && this.AreEqual(x.Result, y.Result);
        }

        protected virtual bool AreEqualGoto(GotoExpression x, GotoExpression y)
        {
            return x.Type == y.Type
                && x.Kind == y.Kind
                && this.AreEqualLabelTarget(x.Target, y.Target)
                && this.AreEqual(x.Value, y.Value);
        }

        protected virtual bool AreEqualLabel(LabelExpression x, LabelExpression y)
        {
            return x == y
                || (x.Type == y.Type
                    && this.AreEqualLabelTarget(x.Target, y.Target));
        }

        protected virtual bool AreEqualLoop(LoopExpression x, LoopExpression y)
        {
            return x.Type == y.Type
                && this.AreEqualLabelTarget(x.ContinueLabel, y.ContinueLabel)
                && this.AreEqualLabelTarget(x.BreakLabel, y.BreakLabel)
                && this.AreEqual(x.Body, y.Body);
        }

        protected virtual bool AreEqualSwitch(SwitchExpression x, SwitchExpression y)
        {
            return x.Type == y.Type
                && this.AreEqualMethodInfo(x.Comparison, y.Comparison)
                && this.AreEqual(x.DefaultBody, y.DefaultBody)
                && this.AreEqual(x.SwitchValue, y.SwitchValue)
                && this.AreEqualCaseList(x.Cases, y.Cases);
        }

        protected virtual bool AreEqualTry(TryExpression x, TryExpression y)
        {
            return x.Type == y.Type
                && this.AreEqualCatchBlockList(x.Handlers, y.Handlers)
                && this.AreEqual(x.Body, y.Body)
                && this.AreEqual(x.Finally, y.Finally);
        }

        protected virtual bool AreEqualNewArray(NewArrayExpression x, NewArrayExpression y)
        {
            return this.AreEqualExpressionList(x.Expressions, y.Expressions);
        }

        protected virtual bool AreEqualInvocation(InvocationExpression x, InvocationExpression y)
        {
            return this.AreEqualExpressionList(x.Arguments, y.Arguments)
                && this.AreEqual(x.Expression, y.Expression);
        }

        /// <summary>
        /// From: https://stackoverflow.com/questions/506096/comparing-object-properties-in-c-sharp.
        /// </summary>
        private static bool PublicInstancePropertiesEqual<T>(T val0, T val1, ICollection<string> ignoredNames = null) where T : class
        {
            Type type;
            object value0;
            object value1;
            if (val0 is object && val1 is object)
            {
                type = val0.GetType();
                foreach (var tmpInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (ignoredNames?.Contains(tmpInfo.Name) == false)
                    {
                        value0 = type.GetProperty(tmpInfo.Name).GetValue(val0, null);
                        value1 = type.GetProperty(tmpInfo.Name).GetValue(val1, null);
                        if (tmpInfo.PropertyType.IsClass && !tmpInfo.PropertyType.Module.ScopeName.Equals("CommonLanguageRuntimeLibrary", StringComparison.OrdinalIgnoreCase) && !PublicInstancePropertiesEqual(value0, value1, ignoredNames) || !ReferenceEquals(value0, value1) && (value0 is null || !value0.Equals(value1)))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            else
            {
                return ReferenceEquals(val0, val1);
            }
        }
    }
}
