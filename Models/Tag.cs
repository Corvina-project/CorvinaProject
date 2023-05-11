using System.Text.Json.Serialization;

namespace MauiAuth0App.Models;

public class Tag
{
    public string deviceId { get; set; }
    public string modelPath { get; set; }
    public string?[] types { get; set; }
    public string?[] header { get; set; }
    public object?[][] data { get; set; }
    public string tagValue { get; set; }
}