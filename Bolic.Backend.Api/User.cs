namespace Bolic.Backend.Api;

public record User{
  public required string id;
  public Analytics analytics = new();
}
