using System;
using WindowsGame1;
namespace Editor
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            bool useEditor = false;
            if (useEditor)
            {
                EditorForm mainForm = new EditorForm();
                mainForm.Show();

                Editor game = new Editor(mainForm);
                game.Run();
            }
            else
            {
                using (Game1 game = new Game1())
                {
                    game.Run();
                }
            }
          
        }
    }
#endif
}

