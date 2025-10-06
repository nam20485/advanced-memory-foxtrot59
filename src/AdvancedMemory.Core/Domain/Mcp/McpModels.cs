namespace AdvancedMemory.Core.Domain.Mcp;

/// <summary>
/// Base MCP request structure.
/// </summary>
public record McpRequest
{
    public required string Method { get; init; }
    public required McpParams Params { get; init; }
    public string? Id { get; init; }
}

/// <summary>
/// MCP request parameters.
/// </summary>
public record McpParams
{
    public string? Name { get; init; }
    public Dictionary<string, object>? Arguments { get; init; }
}

/// <summary>
/// Base MCP response structure.
/// </summary>
public record McpResponse
{
    public required string Id { get; init; }
    public McpResult? Result { get; init; }
    public McpError? Error { get; init; }
}

/// <summary>
/// MCP successful result.
/// </summary>
public record McpResult
{
    public required object Content { get; init; }
    public Dictionary<string, object>? Metadata { get; init; }
}

/// <summary>
/// MCP error information.
/// </summary>
public record McpError
{
    public required int Code { get; init; }
    public required string Message { get; init; }
    public object? Data { get; init; }
}

/// <summary>
/// MCP tool definition.
/// </summary>
public record McpTool
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required McpToolInputSchema InputSchema { get; init; }
}

/// <summary>
/// MCP tool input schema (JSON Schema).
/// </summary>
public record McpToolInputSchema
{
    public required string Type { get; init; }
    public required Dictionary<string, McpProperty> Properties { get; init; }
    public List<string>? Required { get; init; }
}

/// <summary>
/// MCP property definition.
/// </summary>
public record McpProperty
{
    public required string Type { get; init; }
    public required string Description { get; init; }
    public object? Default { get; init; }
    public List<string>? Enum { get; init; }
}
