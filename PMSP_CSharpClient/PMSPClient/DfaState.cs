/*=========================Group/Course Information=========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/

/*=========================Enum Description================================
 * Name : DfaState.
 * Purpose: This enum is used for all DFA states.
 * Version: 1.0
 *==========================================================================*/

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
