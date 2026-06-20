namespace BanqueSang.API.DTOs.Patients;

public class CreatePatientRequestDto
{
    public int? IdService { get; set; }

    public string PatientNom { get; set; } = string.Empty;

    public string PatientPrenom { get; set; } = string.Empty;

    public string PatientAdresse { get; set; } = string.Empty;

    public string PatientTelephone { get; set; } = string.Empty;

    public DateTime PatientDateNaissance { get; set; }

    public string PatientGroupeSanguin { get; set; } = string.Empty;
}