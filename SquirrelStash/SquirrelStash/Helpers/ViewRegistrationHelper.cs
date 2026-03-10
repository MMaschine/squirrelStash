
namespace SquirrelStash.Helpers
{
    /// <summary>
    /// Services extension to simultaneously register View and ViewModel
    /// </summary>
    internal static class ViewRegistrationHelper
    {
        public static IServiceCollection AddViewWithViewModel<TView, TViewModel>(
            this IServiceCollection services,
            ServiceLifetime viewLifetime = ServiceLifetime.Transient,
            ServiceLifetime viewModelLifetime = ServiceLifetime.Transient)
            where TView : class
            where TViewModel : class
        {
            services.Add(new ServiceDescriptor(typeof(TViewModel), typeof(TViewModel), viewModelLifetime));
            services.Add(new ServiceDescriptor(typeof(TView), typeof(TView), viewLifetime));

            return services;
        }
    }
}
