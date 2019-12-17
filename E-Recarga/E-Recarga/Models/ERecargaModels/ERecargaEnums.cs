using System.Collections.Generic;
using System.Linq;

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

    //Tradutor para HTML dos enums
    public static class Enum_Dictionnary
    {
        public static IDictionary<string, string> Translator = new Dictionary<string, string>
        {
            { nameof(PodTypeEnum.Normal), "Normal" },
            { nameof(PodTypeEnum.Fast), "Rápido" },

            { nameof(AppointmentStatusEnum.Pending), "Pendente" },
            { nameof(AppointmentStatusEnum.Ongoing), "A decorrer" },
            { nameof(AppointmentStatusEnum.Completed), "Concluído" },
            { nameof(AppointmentStatusEnum.Absent), "Ausente" },
            { nameof(AppointmentStatusEnum.Cancelled), "Cancelado" },

            { nameof(RoleEnum.Administrator), "Administrador" },
            { nameof(RoleEnum.CompanyManager), "Gestor" },
            { nameof(RoleEnum.Employee), "Funcionário" },
            { nameof(RoleEnum.User).ToString(), "Utilizador" }
        };

        public static string GetKeyFromValue(string value)
        {
            int size = Translator.Count;
            for (int i = 0; i < size; i++)
            {
                if (Translator[Translator.Keys.ElementAt(i)] == value)
                    return Translator.Keys.ElementAt(i);
            }
            return "";
        }
    }
}