using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterBot.Infrastructure.Logging
{
    public interface ILogger
    {
        void Log(string str);
        void Error(string error);

        void Separator();
    }
}
