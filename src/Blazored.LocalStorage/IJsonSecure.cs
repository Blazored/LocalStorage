using System;
using System.Collections.Generic;
using System.Text;

namespace Blazored.LocalStorage
{
    public interface IJsonSecure
    {
        string Encrypt(string json);
        string Decrypt(string cipherText);
    }
}

