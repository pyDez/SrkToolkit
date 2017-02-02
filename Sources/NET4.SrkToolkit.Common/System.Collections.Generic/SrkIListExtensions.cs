﻿// 
// Copyright 2014 SandRock
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

namespace System.Collections.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static  class SrkIListExtensions
    {
        public static T SingleOrCreate<T>(this IList<T> collection, Func<T, bool> predicate, Func<T> create)
            where T : class
        {
            var item = collection.SingleOrDefault(predicate);
            if (item != null)
            {
                return item;
            }
            else
            {
                item = create();
                collection.Add(item);
                return item;
            }
        }

        public static T FirstOrCreate<T>(this IList<T> collection, Func<T, bool> predicate, Func<T> create)
            where T : class
        {
            var item = collection.FirstOrDefault(predicate);
            if (item != null)
            {
                return item;
            }
            else
            {
                item = create();
                collection.Add(item);
                return item;
            }
        }

        public static T LastOrCreate<T>(this IList<T> collection, Func<T, bool> predicate, Func<T> create)
            where T : class
        {
            var item = collection.LastOrDefault(predicate);
            if (item != null)
            {
                return item;
            }
            else
            {
                item = create();
                collection.Add(item);
                return item;
            }
        }
    }
}
