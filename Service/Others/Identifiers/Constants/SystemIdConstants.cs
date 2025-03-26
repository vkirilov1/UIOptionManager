using Service.Others.Identifiers.Model;

namespace Service.Others.Identifiers.Constants
{
    public static class SystemIdConstants
    {
        public sealed class WorkLocation : SystemId<WorkLocation>
        {
            public static readonly WorkLocation Sofia = new(1, "Sofia");
            public static readonly WorkLocation PlovdivOffice = new(2, "Plovdiv");
            public static readonly WorkLocation Varna = new(3, "Varna");

            private WorkLocation(int value, string name) : base(value, name) { }
        }

        public sealed class EmploymentType : SystemId<EmploymentType>
        {
            public static readonly EmploymentType FullTime = new(1, "FullTime");
            public static readonly EmploymentType PartTime = new(2, "Part-Time");
            public static readonly EmploymentType Internship = new(3, "Internship");

            private EmploymentType(int value, string name) : base(value, name) { }
        }
    }
}
