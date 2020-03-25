using System;

namespace Joke.Joke.Tree
{
    public interface IVisitor
    {
        void Visit(IType type) => throw new NotImplementedException();
        void Visit(IntersectionType type);
        void Visit(UnionType type);
        void Visit(TupleType type);
        void Visit(NominalType type);
        void Visit(Class @class);
        void Visit(LambdaType lambda);
        void Visit(ThisType @this);

        void Visit(IExpression expression) => throw new NotImplementedException();

        void Visit(Unit unit);
        void Visit(Field field);
        void Visit(Method method);

    }
}
