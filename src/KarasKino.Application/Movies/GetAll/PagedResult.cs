using System;
using System.Collections.Generic;
using System.Text;

namespace KarasKino.Application.Movies.GetAll;

public record PagedResult<T>(
  List<T> Items,
  int TotalCount,
  int Page,
  int PageSize);
