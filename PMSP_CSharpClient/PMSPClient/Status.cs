/*=========================Group/Course Information=========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/

/*=========================Enum Description================================
 * Name : Status.
 * Purpose: This is the enum used for all HTTP statuses.
 * Version: 1.0
 * Installation Instructions:
 *==========================================================================*/

/*=======================Directives and Pragmas=============================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSPClient
{
    public enum Status
    {
        OK = 200,
        BadRequest = 400,
        Unauthorized = 401,
        InvalidStateTransition = 442,
        InternalServerError = 500,
        NotImplemented = 501
    }
}