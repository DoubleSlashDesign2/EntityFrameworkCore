// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Query.Pipeline
{
    public class QueryCompilationContext2
    {
        public static readonly ParameterExpression QueryContextParameter = Expression.Parameter(typeof(QueryContext), "queryContext");

        private readonly IQueryOptimizerFactory _queryOptimizerFactory;
        private readonly IEntityQueryableTranslatorFactory _entityQueryableTranslatorFactory;
        private readonly IQueryableMethodTranslatingExpressionVisitorFactory _queryableMethodTranslatingExpressionVisitorFactory;
        private readonly IShapedQueryOptimizingExpressionVisitorsFactory _shapedQueryOptimizingExpressionVisitorsFactory;
        private readonly IShapedQueryCompilingExpressionVisitorFactory _shapedQueryCompilingExpressionVisitorFactory;

        public QueryCompilationContext2(
            IModel model,
            IQueryOptimizerFactory queryOptimizerFactory,
            IEntityQueryableTranslatorFactory entityQuerableTranslatorFactory,
            IQueryableMethodTranslatingExpressionVisitorFactory queryableMethodTranslatingExpressionVisitorFactory,
            IShapedQueryOptimizingExpressionVisitorsFactory shapedQueryOptimizingExpressionVisitorsFactory,
            IShapedQueryCompilingExpressionVisitorFactory shapedQueryCompilingExpressionVisitorFactory)
        {
            Model = model;
            _queryOptimizerFactory = queryOptimizerFactory;
            _entityQueryableTranslatorFactory = entityQuerableTranslatorFactory;
            _queryableMethodTranslatingExpressionVisitorFactory = queryableMethodTranslatingExpressionVisitorFactory;
            _shapedQueryOptimizingExpressionVisitorsFactory = shapedQueryOptimizingExpressionVisitorsFactory;
            _shapedQueryCompilingExpressionVisitorFactory = shapedQueryCompilingExpressionVisitorFactory;
        }

        public bool Async { get; internal set; }
        public IModel Model { get; }

        public virtual Func<QueryContext, TResult> CreateQueryExecutor<TResult>(Expression query)
        {
            query = _queryOptimizerFactory.Create(this).Visit(query);

            // Convert EntityQueryable to ShapedQueryExpression
            query = _entityQueryableTranslatorFactory.Create(this).Visit(query);

            query = _queryableMethodTranslatingExpressionVisitorFactory.Create(this).Visit(query);

            foreach (var visitor in _shapedQueryOptimizingExpressionVisitorsFactory.Create(this).GetVisitors())
            {
                query = visitor.Visit(query);
            }

            // Inject actual entity materializer
            // Inject tracking
            query = _shapedQueryCompilingExpressionVisitorFactory.Create(this).Visit(query);

            return Expression.Lambda<Func<QueryContext, TResult>>(
                query,
                QueryContextParameter)
                .Compile();
        }
    }
}
