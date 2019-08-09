using mARkIt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mARkIt
{
    public sealed class App
    {
        private static App s_Instance = null;
        private static object s_LockObj = new Object();

        private App() { }

        public static App Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (s_LockObj)
                    {
                        if (s_Instance == null)
                        {
                            s_Instance = new App();
                        }
                    }
                }

                return s_Instance;
            }
        }    

        private User m_User;

        public static User User
        {
            get
            {
                return Instance.m_User;
            }

            set
            {
                Instance.m_User = value; ;
            }
        }
    }
}
