﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

using Joke.Front.Pony.Lexing;
using Joke.Front.Pony.ParseTree;

namespace Joke.Front.Pony.Visit
{
    public class Reflect
    {
        private Dictionary<System.Type, List<PropertyInfo>> cache = new Dictionary<System.Type, List<PropertyInfo>>();

        public IEnumerable<PtNode> Or(PtNode node)
        {
            foreach (var info in GetProps(node))
            {
                if (typeof(PtNode).IsAssignableFrom(info.PropertyType))
                {
                    var child = (PtNode?)info.GetValue(node);
                    if (child != null)
                    {
                        yield return child;
                    }
                }
                else if (typeof(PonyTokenSpan).IsAssignableFrom(info.PropertyType))
                {
                }
                else if (typeof(bool).IsAssignableFrom(info.PropertyType))
                {
                }
                else if (info.PropertyType.IsEnum)
                {
                }
                else if (typeof(IEnumerable<PtNode>).IsAssignableFrom(info.PropertyType))
                {
                    var value = (IEnumerable<PtNode>?)info.GetValue(node);
                    if (value != null)
                    {
                        foreach (var child in value)
                        {
                            yield return child;
                        }
                    }
                }
                else
                {
                    Debug.Assert(false);
                }
            }
        }

        private List<PropertyInfo> GetProps(PtNode node)
        {
            var type = node.GetType();

            if (!cache.TryGetValue(type, out var props))
            {
                props = GetProps(type);
                cache.Add(type, props);
            }

            return props;
        }

        private List<PropertyInfo> GetProps(System.Type? type)
        {
            var props = new List<PropertyInfo>();
            if (type != null)
            {
                props.AddRange(GetProps(type.BaseType));

                props.AddRange(type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            }

            return props;
        }
    }
}
