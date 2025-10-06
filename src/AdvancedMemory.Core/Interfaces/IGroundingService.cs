using AdvancedMemory.Core.Domain.Entities;
using AdvancedMemory.Core.Domain.ValueObjects;

namespace AdvancedMemory.Core.Interfaces;

/// <summary>
/// Grounding service interface for fact verification and confidence scoring.
/// </summary>
public interface IGroundingService
{
    /// <summary>
    /// Verifies a statement against the knowledge base.
    /// </summary>
    Task<GroundingResult> VerifyStatementAsync(string userId, string statement, CancellationToken cancellationToken = default);

    /// <summary>
    /// Grounds a query result with supporting evidence from the knowledge base.
    /// </summary>
    Task<QueryResult> GroundQueryResultAsync(QueryResult queryResult, CancellationToken cancellationToken = default);

    /// <summary>
    /// Calculates confidence score for a statement based on supporting evidence.
    /// </summary>
    Task<double> CalculateConfidenceScoreAsync(string statement, List<Memory> supportingMemories, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds supporting evidence for a statement in the knowledge base.
    /// </summary>
    Task<List<Evidence>> FindSupportingEvidenceAsync(string userId, string statement, int maxResults = 5, CancellationToken cancellationToken = default);

    /// <summary>
    /// Detects potential contradictions between a statement and existing knowledge.
    /// </summary>
    Task<List<Contradiction>> DetectContradictionsAsync(string userId, string statement, CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of grounding verification.
/// </summary>
public record GroundingResult(
    bool IsGrounded,
    double ConfidenceScore,
    List<Evidence> SupportingEvidence,
    List<Contradiction> Contradictions,
    string? Explanation = null
);

/// <summary>
/// Evidence supporting a statement.
/// </summary>
public record Evidence(
    string Source,
    string Content,
    double RelevanceScore,
    DateTime Timestamp,
    Dictionary<string, string>? Metadata = null
);

/// <summary>
/// Contradiction found in the knowledge base.
/// </summary>
public record Contradiction(
    string ConflictingStatement,
    string Source,
    double ConflictScore,
    string? Explanation = null
);
