namespace CareBaseApi.Enums
{
    public enum UserRole
    {
        Admin = 0,
        User = 2,
        SM = 999
    }
    public enum ConsultationStatus
    {
        Agendado = 0,
        Compareceu = 1,
        NaoCompareceu = 2,
        Reagendado = 3
    }
    public enum PaymentMethod
    {
        Pix = 0,
        Debito = 1,
        Credito = 2,
        Dinheiro = 3
    }

    public enum PaymentStatus
    {
        Pending = 0,
        Paid = 1,
        Refunded = 2,
        Voided = 3
    }

}