/*=========================Group/Course Information=========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/

/*=========================Class Description================================
 * Name : Utilities.
 * Purpose: This class is used for generic utilities and helper functions. used by the application.
 * Version: 1.0
 * Installation Instructions:
 *==========================================================================*/

/*=======================Directives and Pragmas=============================*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PMSPClient
{
    /// <summary>
    /// This class is used for generic utilities and helper functions. used by the application.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Writes a new line to the console.
        /// </summary>
        public static void WriteNewLine()
        {
            Console.WriteLine(Environment.NewLine);
        }

        /// <summary>
        /// Gets an enum value from it description.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
        }

        /// <summary>
        /// Gets an enum description from it's value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescriptionFromValue(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        /// <summary>
        /// Returns a desired cookie from a Set-Cookie header.
        /// </summary>
        /// <param name="setCookie"></param>
        /// <param name="cookieToGet"></param>
        /// <returns></returns>
        public static string GetCookieFromHeader(string setCookie, string cookieToGet, int cookieFoundNumber)
        {
            string[] cookies = setCookie.Split(new Char[] { ',', ';' });
            string selectedCookie = "";
            int currentFoundCookieNumber = 1;
            foreach (string cookie in cookies)
            {
                if (cookie.Contains(cookieToGet))
                {
                    if (cookieFoundNumber == currentFoundCookieNumber)
                    {
                        selectedCookie = cookie;
                        break;
                    }
                    else
                    {
                        currentFoundCookieNumber++;
                    }
                }
            }
            return selectedCookie;
        }
    }
}
