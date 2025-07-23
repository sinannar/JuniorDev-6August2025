using System;
using System.ComponentModel.DataAnnotations;

namespace dotnetwebapi;

public class TestData
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
