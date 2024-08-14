using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FamilyTree.Data;

public static class DbContextExtensions
{
    public static bool HasDuplicate<TEntity>(
         this DbSet<TEntity> dbSet,
         TEntity entity,
         params Expression<Func<TEntity, object>>[] keySelectors) where TEntity : class
    {
        IQueryable<TEntity> query = dbSet;

        foreach (var keySelector in keySelectors)
        {
            var compiledKeySelector = keySelector.Compile();
            var propertyValue = compiledKeySelector(entity);
            query = query.Where(e => EF.Property<object>(e, GetPropertyName(keySelector)) == propertyValue);
        }

        return query.Any();
    }

    private static string GetPropertyName<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertyLambda)
    {
        if (propertyLambda.Body is MemberExpression member)
            return member.Member.Name;

        if (propertyLambda.Body is UnaryExpression unary && unary.Operand is MemberExpression memberExpr)
            return memberExpr.Member.Name;

        throw new ArgumentException("Invalid property expression");
    }
}
