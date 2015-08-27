using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using Popcorn.Helpers;
using Popcorn.Messaging;
using Popcorn.Models.Account;
using RestSharp;

namespace Popcorn.Services.User
{
    /// <summary>
    /// Services used to interacts with user's data
    /// </summary>
    public class UserService
    {
        #region Logger

        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Methods

        #region Method -> CreateUser

        /// <summary>
        /// Create a Popcorn user account
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="firstname">The first name</param>
        /// <param name="lastname">The last name</param>
        /// <param name="password">The password</param>
        /// <param name="email">The email</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>User</returns>
        public async Task<Models.Account.User> CreateUser(string username, string firstname, string lastname,
            string password, string email, CancellationToken ct)
        {
            var watch = Stopwatch.StartNew();

            var user = new Models.Account.User();

            var restClient = new RestClient(Constants.PopcornApiEndpoint);
            var request = new RestRequest("/{segment}", Method.POST);
            request.AddUrlSegment("segment", "api/accounts/create");
            request.AddParameter("username", username);
            request.AddParameter("firstname", firstname);
            request.AddParameter("lastname", lastname);
            request.AddParameter("email", email);
            request.AddParameter("password", password);
            request.AddParameter("confirmpassword", password);

            try
            {
                var response = await restClient.ExecutePostTaskAsync<Models.Account.User>(request, ct);
                if (response.ErrorException != null)
                {
                    watch.Stop();
                    Logger.Error(
                        $"CreateUser: {response.ErrorException.Message}");
                    return user;
                }

                user = response.Data;
            }
            catch (Exception exception) when (exception is TaskCanceledException)
            {
                watch.Stop();
                Logger.Debug(
                    "CreateUser cancelled.");
            }
            catch (Exception exception) when (exception is SocketException || exception is WebException)
            {
                Logger.Error(
                    $"CreateUser: {exception.Message}");
                Messenger.Default.Send(new ManageExceptionMessage(exception));
            }
            catch (Exception exception)
            {
                watch.Stop();
                Logger.Error(
                    $"CreateUser: {exception.Message}");
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Logger.Debug(
                $"CreateUser ({username}, {firstname}, {lastname}, {password}, {email}) in {elapsedMs} milliseconds.");

            return user;
        }

        #endregion

        #region Method -> Signin

        /// <summary>
        /// Signin with a user
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns>Bearer</returns>
        public async Task<Bearer> Signin(Models.Account.User user, CancellationToken ct)
        {
            var watch = Stopwatch.StartNew();

            var bearer = new Bearer();

            var restClient = new RestClient(Constants.PopcornApiEndpoint);
            var request = new RestRequest("/{segment}", Method.POST);
            request.AddUrlSegment("segment", "oauth/token");
            request.AddParameter("username", user.Username);
            request.AddParameter("password", user.Password);
            request.AddParameter("grant_type", "password");

            try
            {
                var response = await restClient.ExecutePostTaskAsync<Bearer>(request, ct);
                if (response.ErrorException != null)
                {
                    watch.Stop();
                    Logger.Error(
                        $"Signin: {response.ErrorException.Message}");
                    return bearer;
                }

                bearer = response.Data;
            }
            catch (Exception exception) when (exception is TaskCanceledException)
            {
                watch.Stop();
                Logger.Debug(
                    "Signin cancelled.");
            }
            catch (Exception exception) when (exception is SocketException || exception is WebException)
            {
                Logger.Error(
                    $"Signin: {exception.Message}");
                Messenger.Default.Send(new ManageExceptionMessage(exception));
            }
            catch (Exception exception)
            {
                watch.Stop();
                Logger.Error(
                    $"Signin: {exception.Message}");
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Logger.Debug(
                $"Signin ({user.Username}, {user.Fullname}, {user.Email}) in {elapsedMs} milliseconds.");

            return bearer;
        }

        #endregion

        #endregion
    }
}