namespace ProfileXMLBuilder.Lib
{
    public enum ProxyType
    {
        AutoConfigUrl,
        Manual
    }

    public enum RoutingPolicyType
    {
        SplitTunnel,
        ForceTunnel
    }

    public enum NativeProtocolType
    {
        Automatic,
        PPTP,
        L2TP,
        IKEv2
    }

    public enum UserMethod
    {
        EAP,
        MSChapv2
    }

    public enum MachineMethod
    {
        Certificate
    }

    public enum AuthenticationTransformConstants
    {
        MD596,
        SHA196,
        SHA256128,
        GCMAES128,
        GCMAES192,
        GCMAES256,
    }

    public enum CipherTransformConstants
    {
        DES,
        DES3,
        AES128,
        AES192,
        AES256,
        GCMAES128,
        GCMAES192,
        GCMAES256
    }

    public enum EncryptionMethod
    {
        DES,
        DES3,
        AES128,
        AES192,
        AES256,
        AES_GCM_128,
        AES_GCM_256
    }

    public enum IntegrityCheckMethod
    {
        MD5,
        SHA196,
        SHA256,
        SHA384
    }

    public enum DHGroup
    {
        Group1,
        Group2,
        Group14,
        ECP256,
        ECP384,
        Group24
    }

    public enum PfsGroup
    {
        PFS1,
        PFS2,
        PFS2048,
        ECP256,
        ECP384,
        PFSMM,
        PFS24
    }

    public enum AuthenticationMethod
    {
        UserEapTls,
        UserEapMschapv2,
        UserPeapTls,
        UserPeapMschapv2,
        UserMschapv2,
        MachineCert
    }
}
