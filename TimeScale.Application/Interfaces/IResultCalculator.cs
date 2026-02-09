
using TimeScale.DataAccess.Entities;

namespace TimeScale.Application.Interfaces
{
    public interface IResultCalculator
    {
        Result Calculate(IEnumerable<ValueRecord> records);
    }

}
