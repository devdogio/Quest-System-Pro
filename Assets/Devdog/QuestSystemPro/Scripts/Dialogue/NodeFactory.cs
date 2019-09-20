using System;
using System.Reflection;
using UnityEngine.Assertions;

namespace Devdog.QuestSystemPro.Dialogue
{
    public static class NodeFactory
    {
        public static NodeBase Create(Type type, params Edge[] edges)
        {
            Assert.IsFalse(type.IsAbstract, "Given type is abstract! Can't create a new instance of " + type);

            var constructor = type.GetConstructor(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance,
                    null,
                    new Type[0], // new [] { typeof(uint) }
                    null
                ); 

            if (constructor == null)
            {
                throw new ArgumentException("Given type: " + type + " does not have an empty constructor", "type");
            }

            var node = (NodeBase) constructor.Invoke(new object[0]);
            node.edges = edges ?? new Edge[0];
            return node;
        }

        public static T Create<T>(params Edge[] edges) where T : NodeBase
        {
            return (T) Create(typeof (T), edges);
        }
    }
}
