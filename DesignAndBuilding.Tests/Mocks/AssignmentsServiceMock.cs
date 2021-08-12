namespace DesignAndBuilding.Tests.Mocks
{
    using global::DesignAndBuilding.Services;
    using Moq;
    using System.Collections.Generic;

    public class AssignmentsServiceMock
    {
        public static IAssignmentsService Instance
        {
            get
            {
                var mock = new Mock<IAssignmentsService>();

                mock.Setup(dtp => dtp.GetAllUsersBidInAssignment(1)).Returns(new List<string>() { "1", "2", "3" });

                return mock.Object;
            }
        }
    }
}
