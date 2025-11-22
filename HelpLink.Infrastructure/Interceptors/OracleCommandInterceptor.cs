using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace HelpLink.Infrastructure.Interceptors;

public class OracleCommandInterceptor : DbCommandInterceptor
{
    private void FixBooleanLiterals(DbCommand command)
    {
        if (command.CommandText.Contains("TRUE") || command.CommandText.Contains("FALSE") || 
            command.CommandText.Contains("True") || command.CommandText.Contains("False"))
        {
            command.CommandText = command.CommandText
                .Replace("TRUE", "1")
                .Replace("FALSE", "0")
                .Replace("True", "1")
                .Replace("False", "0");
        }
    }

    public override InterceptionResult<int> NonQueryExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<int> result)
    {
        FixBooleanLiterals(command);
        return base.NonQueryExecuting(command, eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        FixBooleanLiterals(command);
        return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        FixBooleanLiterals(command);
        return base.ReaderExecuting(command, eventData, result);
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        FixBooleanLiterals(command);
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override InterceptionResult<object> ScalarExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<object> result)
    {
        FixBooleanLiterals(command);
        return base.ScalarExecuting(command, eventData, result);
    }

    public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<object> result,
        CancellationToken cancellationToken = default)
    {
        FixBooleanLiterals(command);
        return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
    }
}