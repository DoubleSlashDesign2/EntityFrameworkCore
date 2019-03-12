// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Query.Pipeline
{
    public class QueryCompilationContextFactory2 : IQueryCompilationContextFactory2
    {
        private readonly IModel _model;
        private readonly IQueryOptimizerFactory _queryOptimizerFactory;
        private readonly IEntityQueryableTranslatorFactory _entityQueryableTranslatorFactory;
        private readonly IQueryableMethodTranslatingExpressionVisitorFactory _queryableMethodTranslatingExpressionVisitorFactory;
        private readonly IShapedQueryOptimizingExpressionVisitorsFactory _shapedQueryOptimizingExpressionVisitorsFactory;
        private readonly IShapedQueryCompilingExpressionVisitorFactory _shapedQueryCompilingExpressionVisitorFactory;

        public QueryCompilationContextFactory2(
            IModel model,
            IQueryOptimizerFactory queryOptimizerFactory,
            IEntityQueryableTranslatorFactory entityQueryableTranslatorFactory,
            IQueryableMethodTranslatingExpressionVisitorFactory queryableMethodTranslatingExpressionVisitorFactory,
            IShapedQueryOptimizingExpressionVisitorsFactory shapedQueryOptimizingExpressionVisitorsFactory,
            IShapedQueryCompilingExpressionVisitorFactory shapedQueryCompilingExpressionVisitorFactory)
        {
            _model = model;
            _queryOptimizerFactory = queryOptimizerFactory;
            _entityQueryableTranslatorFactory = entityQueryableTranslatorFactory;
            _queryableMethodTranslatingExpressionVisitorFactory = queryableMethodTranslatingExpressionVisitorFactory;
            _shapedQueryOptimizingExpressionVisitorsFactory = shapedQueryOptimizingExpressionVisitorsFactory;
            _shapedQueryCompilingExpressionVisitorFactory = shapedQueryCompilingExpressionVisitorFactory;
        }

        public QueryCompilationContext2 Create(bool async)
        {
            var queryCompilationContext = new QueryCompilationContext2(
                _model,
                _queryOptimizerFactory,
                _entityQueryableTranslatorFactory,
                _queryableMethodTranslatingExpressionVisitorFactory,
                _shapedQueryOptimizingExpressionVisitorsFactory,
                _shapedQueryCompilingExpressionVisitorFactory)
            {
                Async = async
            };

            return queryCompilationContext;
        }
    }
}
