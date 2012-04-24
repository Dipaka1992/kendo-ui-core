namespace KendoUI.Mvc.UI.Fluent
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Extensions;
    using Infrastructure;

    public class GridFilterDescriptorFactory<TModel> : IHideObjectMembers where TModel : class
    {
        public GridFilterDescriptorFactory(IList<CompositeFilterDescriptor> filters)
        {
            Guard.IsNotNull(filters, "filters");

            Filters = filters;
        }

        protected IList<CompositeFilterDescriptor> Filters { get; private set; }

        public virtual GridFilterEqualityDescriptorBuilder<bool> Add(Expression<Func<TModel, bool>> expression)
        {
            var filter = CreateFilter(expression);

            return new GridFilterEqualityDescriptorBuilder<bool>(filter);
        }

        public virtual GridFilterEqualityDescriptorBuilder<bool?> Add(Expression<Func<TModel, bool?>> expression)
        {
            var filter = CreateFilter(expression);

            return new GridFilterEqualityDescriptorBuilder<bool?>(filter);
        }

        public virtual GridFilterComparisonDescriptorBuilder<TValue> Add<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            var filter = CreateFilter(expression);

            return new GridFilterComparisonDescriptorBuilder<TValue>(filter);
        }

        public virtual GridFilterStringDescriptorBuilder Add(Expression<Func<TModel, string>> expression)
        {
            var filter = CreateFilter(expression);

            return new GridFilterStringDescriptorBuilder(filter);
        }

        public virtual void AddRange(IEnumerable<IFilterDescriptor> filters)
        {
            foreach (var filter in filters)
            {
                var composite = filter as CompositeFilterDescriptor;

                if (composite == null)
                {
                    composite = new CompositeFilterDescriptor
                    {
                        LogicalOperator = FilterCompositionLogicalOperator.And
                    };

                    composite.FilterDescriptors.Add(filter);
                }

                Filters.Add(composite);
            }
        }

        protected virtual CompositeFilterDescriptor CreateFilter<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            var composite = new CompositeFilterDescriptor
            {
                LogicalOperator = FilterCompositionLogicalOperator.And
            };

            var descriptor = new FilterDescriptor { Member = expression.MemberWithoutInstance() };

            composite.FilterDescriptors.Add(descriptor);

            Filters.Add(composite);

            return composite;
        }
    }
}