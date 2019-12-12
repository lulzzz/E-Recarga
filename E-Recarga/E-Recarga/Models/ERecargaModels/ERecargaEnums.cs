namespace E_Recarga.Models
{
    public enum PodTypeEnum
    {
        Normal,
        Fast
    }

    public enum AppointmentStatusEnum
    {
        Pending,
        Ongoing,
        Completed,
        Absent,
        Cancelled
    }

    public enum RoleEnum
    {
        Administrator,      //Manager (Adds and removes companies)
        CompanyManager,     //Can assign employees to a station, add, remove and edit stations (Can do everything a Employee can do).
        Employee,           //Can see, Edit and cancel appointments. Appointments cancelled by the Employee do not charge the user. Can also flag pods as inactive if they are broken
        User
    }
}