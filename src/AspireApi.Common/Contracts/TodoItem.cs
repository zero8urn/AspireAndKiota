using System;
using System.ComponentModel.DataAnnotations;

namespace AspireApi.Common.Contracts;

public record TodoItem([Required] Guid Id, [Required] string Name);
