using System;
using XTMF2;
using XTMF2.Controllers;
using XTMF2.UnitTests.Modules;

namespace XTMF2.Web.Testing.Demo
{
    /// <summary>
    /// Demo Program that populates XTMF2 with a model system and project for testing.
    /// </summary>
    class DemoProgram
    {
        private static XTMFRuntime Runtime;
        public static ProjectController ProjectController;
        static void Main(string[] args)
        {
            Runtime = XTMFRuntime.CreateRuntime();
            ProjectController = Runtime.ProjectController;
            CreateDemoModelSystem(Runtime.UserController.GetUserByName("local"), "DemoProject","DemoModelSystem");
        }

        /// <summary>
        /// Creates a demo model system under the passed project and model system name.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        static void CreateDemoModelSystem(User user, string projectName, string modelSystemName)
        {
            ProjectController.CreateNewProject(user, projectName, out var projectSession, out var error);
            projectSession.CreateNewModelSystem(user, modelSystemName, out var modelSystem, out error);
            projectSession.EditModelSystem(user, modelSystem, out var modelSystemSession, out error);

            modelSystemSession.AddModelSystemStart(user, modelSystemSession.ModelSystem.GlobalBoundary, "TestStart",
                new Rectangle(10, 10, 100, 100),
                out var start, out error);
            modelSystemSession.AddNode(user, modelSystemSession.ModelSystem.GlobalBoundary, "TestNode1",
                typeof(SimpleTestModule), new Rectangle(10, 10, 100, 100), out var node, out error);
            modelSystemSession.AddLink(user, start, start.Hooks[0], node, out var link, out error);
            modelSystemSession.AddBoundary(user, modelSystemSession.ModelSystem.GlobalBoundary, "TestBoundary1",
                out var boundary, out error);
            modelSystemSession.AddNode(user, boundary, "TestNode2",
                typeof(SimpleTestModule), new Rectangle(10, 10, 100, 100), out var testNode2, out error);
            modelSystemSession.Save(out error);
            Runtime.Shutdown();
        }
    }
}
