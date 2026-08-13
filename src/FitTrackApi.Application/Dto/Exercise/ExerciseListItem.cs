using System;
using System.Collections.Generic;
using System.Text;

namespace FitTrackApi.Application.Dto.Exercise;
public class ExerciseListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
}
