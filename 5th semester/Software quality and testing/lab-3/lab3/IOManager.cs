using System;

namespace lab3
{
    public abstract class IOManager
    {
        public abstract string ParseFile(string file);

        public abstract string AskFileName();

        public abstract void GiveAnswer(string text);
    }
}
