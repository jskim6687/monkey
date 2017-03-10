using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monkey.Models
{
    public interface IFileRepository
    {
        void getOfileInfo(string oFileName);
        void getNfileInfo(string nFileName);
    }
}
