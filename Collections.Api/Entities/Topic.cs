using NpgsqlTypes;

namespace Collections.Api.Entities;

public class Topic
{
    public int Id { get; set; }

    public string EnName { get; set; }

    public string RuName { get; set; }

    public NpgsqlTsVector SimpleSearchVector { get; set; }
}
