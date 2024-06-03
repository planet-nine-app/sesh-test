﻿using SessionlessNET.Impl;

namespace SessionlessNET.Models;

/// <summary> Vault to be used with <see cref="ISessionless"/>
/// <list type="bullet"><item>
/// This is used since C# has no built-in secure storage
/// <br/> so each platform should implement their own methods (<see cref="Get"/> | <see cref="Save"/>)
/// </item></list>
/// </summary>
public interface IVault {
    /// <summary> Get the stored <see cref="KeyPairHex"/> </summary>
    public KeyPairHex? Get();
    /// <summary> Store a <see cref="KeyPairHex"/> </summary>
    public void Save(KeyPairHex pair);
}
