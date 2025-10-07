# Sample data seeded successfully!

=== Query Examples ===

1. Training Courses with Modules:
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT t.id, t.created_at, t.description, t.duration_hours, t.name, t.price, t.requires_vm, t.updated_at, m.id, m.created_at, m.description, m.duration_hours, m.name, m.order_number, m.training_course_id, m.updated_at
      FROM training_courses AS t
      LEFT JOIN modules AS m ON t.id = m.training_course_id
      ORDER BY t.id
   - .NET 9 Fundamentals (40h, $2500.00)
     1. Introduction to .NET (8h)
     2. C# Fundamentals (16h)
     3. ASP.NET Core (16h)
   - Cloud Architecture (32h, $3000.00)

2. Customers and Students:
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT c.id, c.address, c.created_at, c.email, c.name, c.phone, c.updated_at, s.id, s.created_at, s.customer_id, s.email, s.name, s.phone, s.updated_at
      FROM customers AS c
      LEFT JOIN students AS s ON c.id = s.customer_id
      ORDER BY c.id
   - Acme Corporation (contact@acme.com)
     Students: Alice Developer, Bob Engineer

3. Virtual Machines with Usage:
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT v.id, v.created_at, v.ip_address, v.name, v.status, v.training_course_id, v.updated_at, v.vm_type_id, v0.id, v0.created_at, v0.description, v0.name, v0.updated_at, d.id, d.cost, d.created_at, d.hours_used, d.usage_date, d.virtual_machine_id
      FROM virtual_machines AS v
      INNER JOIN vm_types AS v0 ON v.vm_type_id = v0.id
      LEFT JOIN daily_usage_statistics AS d ON v.id = d.virtual_machine_id
      ORDER BY v.id, v0.id
   - training-vm-001 (Windows) - Running
     Total Usage: 14.5h, Total Cost: $58.00
   - training-vm-002 (Linux) - Running
     Total Usage: 7.0h, Total Cost: $28.00

4. Students with RDP Access:
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT s.id, s.created_at, s.customer_id, s.email, s.name, s.phone, s.updated_at, s0.id, s0.created_at, s0.file_name, s0.file_path, s0.student_id, s0.updated_at, s0.virtual_machine_id, s0.id0, s0.created_at0, s0.ip_address, s0.name, s0.status, s0.training_course_id, s0.updated_at0, s0.vm_type_id
      FROM students AS s
      LEFT JOIN (
          SELECT r.id, r.created_at, r.file_name, r.file_path, r.student_id, r.updated_at, r.virtual_machine_id, v.id AS id0, v.created_at AS created_at0, v.ip_address, v.name, v.status, v.training_course_id, v.updated_at AS updated_at0, v.vm_type_id
          FROM rdp_files AS r
          INNER JOIN virtual_machines AS v ON r.virtual_machine_id = v.id
      ) AS s0 ON s.id = s0.student_id
      ORDER BY s.id, s0.id
   - Alice Developer (alice@acme.com)
     RDP: training-vm-001.rdp -> training-vm-001
   - Bob Engineer (bob@acme.com)
     RDP: training-vm-002.rdp -> training-vm-002

5. Pending Invoices:
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT b.id, b.created_at, b.customer_id, b.due_date, b.invoice_date, b.invoice_number, b.status, b.total_amount, b.updated_at, c.id, c.address, c.created_at, c.email, c.name, c.phone, c.updated_at
      FROM billing_invoices AS b
      INNER JOIN customers AS c ON b.customer_id = c.id
      WHERE b.status = 'Pending'
   - INV-2024-001 - Acme Corporation
     Amount: $5500.00, Due: 2025-11-06

6. VM Types and Options:
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT v.id, v.created_at, v.description, v.name, v.updated_at, v0.id, v0.created_at, v0.iso_vhd, v0.name, v0.offer, v0.sku, v0.updated_at, v0.version, v0.vm_type_id
      FROM vm_types AS v
   - INV-2024-001 - Acme Corporation
     Amount: $5500.00, Due: 2025-11-06

6. VM Types and Options:
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT v.id, v.created_at, v.description, v.name, v.updated_at, v0.id, v0.created_at, v0.iso_vhd, v0.name, v0.offer, v0.sku, v0.updated_at, v0.version, v0.vm_type_id
   - INV-2024-001 - Acme Corporation
     Amount: $5500.00, Due: 2025-11-06

6. VM Types and Options:
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
   - INV-2024-001 - Acme Corporation
     Amount: $5500.00, Due: 2025-11-06

   - INV-2024-001 - Acme Corporation
    # Sample data seeded successfully!
   - INV-2024-001 - Acme Corporation

   - INV-2024-001 - Acme Corporation

     Amount: $5500.00, Due: 2025-11-06

   - INV-2024-001 - Acme Corporation

   - INV-2024-001 - Acme Corporation
