using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mobile_App_Development_Project
{
    public class AppSettings
    {
        private int timer;
        private bool isLocationActivated;

        public AppSettings()
        {
            IsLocationActivated = true;
        }

        // Properties
        public int Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        public bool IsLocationActivated
        {
            get { return isLocationActivated; }
            set { isLocationActivated = value; }
        }

        // Methods
        public override string ToString()
        {
            return "AppSettings=[Timer=" + Timer + ", IsLocationActivated=" + IsLocationActivated + "]";
        }
    }
}
