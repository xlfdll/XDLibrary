using System;

using Renci.SshNet;

namespace Xlfdll.Network.SecureShell
{
    public static class SSHAuthHelper
    {
        public static ConnectionInfo CreateConnectionInfo(String address, String userName, String password)
        {
            // Use Keyboard-interactive authentication method to avoid "no suitable auth method" exception
            KeyboardInteractiveAuthenticationMethod kbdAuthMethod = new KeyboardInteractiveAuthenticationMethod(userName);

            kbdAuthMethod.AuthenticationPrompt += (sender, e) =>
            {
                foreach (var prompt in e.Prompts)
                {
                    if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                    {
                        prompt.Response = password;
                    }
                }
            };

            PasswordAuthenticationMethod pwdAuthMethod = new PasswordAuthenticationMethod(userName, password);

            return new ConnectionInfo(address, userName, kbdAuthMethod, pwdAuthMethod);
        }

        public static void DisposeConnectionInfo(ConnectionInfo connectionInfo)
        {
            foreach (var method in connectionInfo.AuthenticationMethods)
            {
                if (method is IDisposable m)
                {
                    m.Dispose();
                }
            }
        }
    }
}