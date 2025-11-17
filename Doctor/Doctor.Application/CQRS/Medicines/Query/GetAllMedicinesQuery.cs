using MediatR;
using Doctor.Domain.Entities;

public class GetAllMedicinesQuery : IRequest<IEnumerable<Medicine>>
{
}
