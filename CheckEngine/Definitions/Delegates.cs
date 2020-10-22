namespace ECMPS.Checks.CheckEngine.Definitions
{

  /// <summary>
  /// Delegate used in Check Engine Run states to pass method to perform additional initialization
  /// specific to a particular process.
  /// </summary>
  /// <returns></returns>
  public delegate bool dAdditionalInitialization(); 

}