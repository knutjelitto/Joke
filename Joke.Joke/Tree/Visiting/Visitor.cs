using System;

namespace Joke.Joke.Tree
{
    public abstract class Visitor : IVisitor
    {
        public virtual void Visit(IntersectionType type)
        {
            throw new NotImplementedException();
        }

        public virtual void Visit(UnionType type)
        {
            throw new NotImplementedException();
        }

        public virtual void Visit(TupleType type)
        {
            throw new NotImplementedException();
        }

        public virtual void Visit(NominalType type)
        {
            throw new NotImplementedException();
        }

        public virtual void Visit(Class @class)
        {
            throw new NotImplementedException();
        }

        public virtual void Visit(LambdaType lambda)
        {
            throw new NotImplementedException();
        }

        public virtual void Visit(ThisType @this)
        {
            throw new NotImplementedException();
        }

        public virtual void Visit(Unit unit)
        {
            throw new NotImplementedException();
        }

        public virtual void Visit(Field field)
        {
            throw new NotImplementedException();
        }

        public virtual void Visit(Method method)
        {
            throw new NotImplementedException();
        }
    }
}
