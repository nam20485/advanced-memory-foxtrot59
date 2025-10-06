namespace AdvancedMemory.Core.Interfaces;

/// <summary>
/// Embedding service interface for generating vector embeddings.
/// </summary>
public interface IEmbeddingService
{
    /// <summary>
    /// Generates an embedding vector for the given text.
    /// </summary>
    Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates embeddings for multiple texts in batch.
    /// </summary>
    Task<List<float[]>> GenerateEmbeddingsBatchAsync(List<string> texts, CancellationToken cancellationToken = default);

    /// <summary>
    /// Calculates cosine similarity between two embedding vectors.
    /// </summary>
    double CalculateSimilarity(float[] embedding1, float[] embedding2);

    /// <summary>
    /// Gets the dimensionality of embeddings produced by this service.
    /// </summary>
    int GetEmbeddingDimensions();
}
