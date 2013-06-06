/*=======================Directives and Pragmas=============================*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSPClient
{
    public enum DfaState
    {
        [Description("pmsp-state=wait.login")]
        AwaitingLogin,
        [Description("pmsp-state=idle")]
        Idle,
        [Description("pmsp-state=wait.list.choice")]
        AwaitingListChoice,
        [Description("pmsp-state=wait.file.choice")]
        AwaitingFileChoice
    }
}
