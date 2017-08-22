using System;
using System.Collections.Generic;
using System.Text;
using WFEngine.Utility.Models;

namespace WFEngine.Utility.Activity
{
    public class AssignersActivity
    {
        public static List<string> FindUsersByNames(List<string> names)
        {
            List<string> users = new List<string>();
            if (names != null && names.Count >0)
            {

            }

            return users;
        }

        public static List<string> FindUsersByGroups(List<string> groups)
        {
            List<string> users = new List<string>();
            if (groups != null && groups.Count > 0)
            {

            }

            return users;
        }

        public static List<string> FindUsersByBuiltinProperty(List<string> Properties)
        {
            List<string> users = new List<string>();
            if (Properties != null && Properties.Count > 0)
            {

            }

            return users;
        }

        public static List<string> FindUsers(ApproversModel approvers)
        {
            List<string> users = new List<string>();
            users.AddRange(FindUsersByNames(approvers.Person));
            users.AddRange(FindUsersByGroups(approvers.Group));
            users.AddRange(FindUsersByBuiltinProperty(approvers.Default));

            return users;
        }

        public enum UserBuiltinProperty
        {
            Manager,
            ManagerII,
        }
    }
}
