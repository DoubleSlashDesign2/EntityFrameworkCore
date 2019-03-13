﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.Pipeline
{
    public class QueryOptimizer
    {
        public Expression Visit(Expression query)
        {
            query = new NullCheckRemovingExpressionVisitor().Visit(query);
            return query;
        }
    }
}
