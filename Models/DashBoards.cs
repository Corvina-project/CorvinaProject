using System.Text.Json.Serialization;

namespace MauiAuth0App.Models;

public class DashBoards
{
    [JsonPropertyName("number")]
    public int Number { get; set; }
    [JsonPropertyName("data")]
    public List<DashBoard> Data { get; set; }
    [JsonPropertyName("totalElements")]
    public int TotalElements { get; set; }
    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
    [JsonPropertyName("last")]
    public bool Last { get; set; }
}

public class DashBoard
{
    [JsonPropertyName("owner")]
    public string Owner { get; set; }
    [JsonPropertyName("orgResourceId")]
    public string OrgResourceId { get; set; }
    [JsonPropertyName("realmId")]
    public string RealmId { get; set; }
    [JsonPropertyName("manifest")]
    public Manifest Manifest { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("creationDate")]
    public long CreationDate { get; set; }
    [JsonPropertyName("version")]
    public string Version { get; set; }
    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }
    [JsonPropertyName("readers")]
    public object[] Readers { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("editors")]
    public object[] Editors { get; set; }
    [JsonPropertyName("updatedAt")]
    public long UpdatedAt { get; set; }
}

public class Manifest
{
    [JsonPropertyName("assets")]
    public Asset[] Assets { get; set; }
}

public class Asset
{
    [JsonPropertyName("generation")]
    public int Generation { get; set; }
    [JsonPropertyName("singedURL")]
    public string SingedURL { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("counter")]
    public int Counter { get; set; }
}