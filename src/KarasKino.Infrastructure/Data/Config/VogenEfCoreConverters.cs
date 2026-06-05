using KarasKino.Core.ContributorAggregate;
using Vogen;

namespace KarasKino.Infrastructure.Data.Config;

[EfCoreConverter<ContributorId>]
[EfCoreConverter<ContributorName>]
internal partial class VogenEfCoreConverters;
